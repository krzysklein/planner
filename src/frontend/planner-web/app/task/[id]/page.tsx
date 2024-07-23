"use client";

export default function Page({ params }: { params: { id: string } }) {
  return <div>Task: {params.id}</div>;
}
