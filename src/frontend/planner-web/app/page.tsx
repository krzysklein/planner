"use client";

import { useContext, useState } from "react";
import { TaskSearchResultResponse } from "../api/models/TaskSearchResultResponse";
import { AppContext } from "./context";
import TaskList from "./tasklist";

export default function Page() {
  const context = useContext(AppContext);
  const [searchPhrase, setSearchPhrase] = useState("");
  const [searchResultResponse, setSearchResultResponse] =
    useState<TaskSearchResultResponse>();

  async function search() {
    setSearchResultResponse(await context.api.searchTasks(searchPhrase, ""));
  }

  return (
    <main className="flex min-h-screen flex-col items-center p-4">
      <div>
        <input
          value={searchPhrase}
          onChange={(e) => setSearchPhrase(e.target.value)}
        />{" "}
        <button onClick={search}>Search</button>
      </div>
      <p>Tasks:</p>
      <TaskList taskSearchResultResponse={searchResultResponse}></TaskList>
    </main>
  );
}
