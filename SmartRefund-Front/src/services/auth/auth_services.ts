import { ApiLoginResponse, LoginForm } from "@/types/auth/login";
import { getCookie, removeCookie } from "@/utils/helpers/manageCookies";
import { AxiosPromise } from "axios";
import { api } from "../api";

export const AuthServices = {
  login(loginForm: LoginForm): AxiosPromise<ApiLoginResponse> {
    return api.post(`/login`, loginForm);
  },

  logout(): void {
    removeCookie();
  },

  async isAuthenticated(): Promise<boolean> {
    try {
      const cookie = await getCookie();
      return !!cookie;
    } catch (error) {
      return false;
    }
  },
};
