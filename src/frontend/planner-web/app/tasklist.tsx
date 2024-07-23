"use client";

import Link from "next/link";
import { TaskSearchResultResponse } from "../api/models/TaskSearchResultResponse";

export default function Component({
  taskSearchResultResponse,
}: {
  taskSearchResultResponse?: TaskSearchResultResponse;
}) {
  return (
    <div>
      {taskSearchResultResponse &&
        taskSearchResultResponse.items &&
        taskSearchResultResponse.items.map((task) => (
          <div key={task.id}>
            <Link href={`/task/${task.id}`}>{task.name}</Link>
          </div>
        ))}
    </div>
  );
}
