import { createContext } from "react";
import ApiService from "../api/ApiService";

export const AppContext = createContext({
  api: new ApiService(),
});
