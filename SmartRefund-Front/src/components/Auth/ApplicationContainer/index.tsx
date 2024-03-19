"use client";

import { lightTheme } from "@/app/theme/themes";
import { AuthProvider } from "@/hooks/useAuth";
import { checkIsPublicRoute } from "@/utils/helpers/checkIsPublicRoute";
import { ThemeProvider } from "@mui/material";
import { usePathname } from "next/navigation";

import ToastContainer from "@/components/ToastContainer";
import { useEffect, useState } from "react";
import { PrivateRoute } from "../PrivateRoute";

export default function ApplicationContainer({
  children,
}: {
  children: React.ReactNode;
}) {
  const pathname = usePathname();
  const [isPublicPage, setIsPublicPage] = useState<boolean>(false);

  useEffect(() => {
    setIsPublicPage(checkIsPublicRoute(pathname));
  }, [pathname]);

  return (
    <AuthProvider>
      <ToastContainer />
      <ThemeProvider theme={lightTheme}>
        {isPublicPage && children}
        {!isPublicPage && <PrivateRoute>{children}</PrivateRoute>}
      </ThemeProvider>
    </AuthProvider>
  );
}
