import { TaskDetailsResponse } from "./models/TaskDetailsResponse";
import { TaskSearchResultResponse } from "./models/TaskSearchResultResponse";

export default class ApiService {
  private baseUrl = "http://localhost:5211";

  async getTaskById(id: string): Promise<TaskDetailsResponse> {
    const response = await fetch(`${this.baseUrl}/tasks/${id}`);
    return await response.json();
  }

  async searchTasks(
    searchPhrase: string,
    state: string,
    pageNumber: number = 0,
    pageSize: number = 10
  ): Promise<TaskSearchResultResponse> {
    const queryParams = new URLSearchParams({
      page_number: pageNumber.toString(),
      page_size: pageSize.toString(),
    });
    if (searchPhrase) queryParams.append("search_phrase", searchPhrase);
    if (state) queryParams.append("state", state);

    const response = await fetch(`${this.baseUrl}/tasks/search?${queryParams}`);
    return await response.json();
  }
}
