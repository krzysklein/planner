import { TaskSearchResultItemResponse } from "./TaskSearchResultItemResponse";

export interface TaskSearchResultResponse {
  totalCount: number;
  items: TaskSearchResultItemResponse[];
}
