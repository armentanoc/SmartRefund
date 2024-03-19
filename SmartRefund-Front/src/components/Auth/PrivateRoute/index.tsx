"use client";

import { APP_ROUTES } from "@/utils/constants/app-routes";
import { isAuthenticated } from "@/utils/helpers/manageCookies";
import { useRouter } from "next/navigation";
import { ReactNode, useEffect, useState } from "react";

type PrivateRouteProps = {
  children: ReactNode;
};

export const PrivateRoute = ({ children }: PrivateRouteProps) => {
  const { push } = useRouter();

  const [isUserAuthenticated, setIsUserAuthenticated] = useState<
    null | boolean
  >(null);

  useEffect(() => {
    async function checkAuthentication() {
      const authenticated = await isAuthenticated();
      setIsUserAuthenticated(authenticated);
    }

    checkAuthentication();
  }, []);

  useEffect(() => {
    if (isUserAuthenticated !== null && isUserAuthenticated === false) {
      push(APP_ROUTES.public.login);
    }
  }, [isUserAuthenticated, push]);

  return (
    <>
      {!isUserAuthenticated && null}
      {isUserAuthenticated && children}
    </>
  );
};
