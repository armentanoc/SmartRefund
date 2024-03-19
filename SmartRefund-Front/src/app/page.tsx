"use client";

import { APP_ROUTES } from "@/utils/constants/app-routes";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

export default function Home() {
  const { push } = useRouter();

  useEffect(() => {
    push(APP_ROUTES.public.login);
  }, []);

  return;
}
