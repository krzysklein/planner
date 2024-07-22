using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Text.Json;
using Tasks.Application;
using Tasks.Application.Dto;

namespace Tasks.Api.Extensions
{
    public static class EitherExtensions
    {
        public static IActionResult ToActionResult<TError>(
            this Either<TError, Unit> result,
            Func<IActionResult>? defaultValidActionResult = null,
            Func<TError, IActionResult>? defaultErrorActionResult = null)
            where TError : Error
        {
            return result
                .Right(_ => defaultValidActionResult != null ? defaultValidActionResult() : new NoContentResult())
                .Left(e => defaultErrorActionResult != null ? defaultErrorActionResult(e) : MapErrorToHttpActionResult(e));
        }

        public static IActionResult ToActionResult<TError, TResult>(
            this Either<TError, TResult> result,
            Func<TResult, IActionResult>? defaultValidActionResult = null,
            Func<TError, IActionResult>? defaultErrorActionResult = null)
            where TError : Error
        {
            return result
                .Right(v => defaultValidActionResult != null ? defaultValidActionResult(v) : new JsonResult(v))
                .Left(e => defaultErrorActionResult != null ? defaultErrorActionResult(e) : MapErrorToHttpActionResult(e));
        }

        private static IActionResult MapErrorToHttpActionResult(Error error) =>
            new ContentResult()
            {
                StatusCode = (int)error.StatusCode,
                ContentType = MediaTypeNames.Application.Json,
                Content = JsonSerializer.Serialize(new ErrorResponse(error.Message, error.Details))
            };
    }
}
