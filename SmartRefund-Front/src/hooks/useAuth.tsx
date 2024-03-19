import { AuthServices } from "@/services/auth/auth_services";
import { LoginForm } from "@/types/auth/login";
import { removeCookie, saveCookie } from "@/utils/helpers/manageCookies";
import { createContext, useCallback, useContext } from "react";

type IAuthContextData = {
  handleLogin: (loginForm: LoginForm) => Promise<boolean>;
  handleLogout: () => void;
};

const AuthContext = createContext({} as IAuthContextData);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const handleLogin = async (loginForm: LoginForm) => {
    try {
      const { data } = await AuthServices.login(loginForm);
      if (data.token !== "") {
        saveCookie(data.token, data.userType);
      } else {
        throw new Error(
          "Token not found for the provided user type. Check the .env.local file",
        );
      }

      return true;
    } catch (error) {
      console.error("error:", error);
      return false;
    }
  };

  const handleLogout = useCallback(() => {
    removeCookie();
  }, []);

  return (
    <AuthContext.Provider value={{ handleLogin, handleLogout }}>
      {children}
    </AuthContext.Provider>
  );
};

const useAuth = () => {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth must be used with AuthProvider");
  }

  return context;
};

export default useAuth;
