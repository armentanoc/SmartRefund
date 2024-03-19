"use server";

import { cookies } from "next/headers";

const APP_KEY = "SMART_REFUND";

export const saveCookie = (userToken: string, userType: string) => {
  cookies().set(`${APP_KEY}_USER_TOKEN`, userToken);
  cookies().set(`${APP_KEY}_USER_TYPE`, userType);
};

export const removeCookie = () => {
  cookies().set(`${APP_KEY}_USER_TOKEN`, "", {
    maxAge: 0,
  });
  cookies().set(`${APP_KEY}_USER_TYPE`, "", {
    maxAge: 0,
  });
};

export const getCookie = () => {
  return {
    userToken: cookies().get(`${APP_KEY}_USER_TOKEN`),
    userType: cookies().get(`${APP_KEY}_USER_TYPE`),
  };
};

export const isAuthenticated = async () => {
  const cookieValue = await getCookie();
  return cookieValue.userToken !== undefined;
};
