"use client";

import useAuth from "@/hooks/useAuth";
import { LoginForm } from "@/types/auth/login";
import { APP_ROUTES } from "@/utils/constants/app-routes";
import { isAuthenticated } from "@/utils/helpers/manageCookies";
import { yupResolver } from "@hookform/resolvers/yup";
import { AccountCircle, LockRounded } from "@mui/icons-material";
import { Button, InputAdornment, TextField, Typography } from "@mui/material";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { SubmitHandler, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as yup from "yup";

const loginSchema = yup.object().shape({
  username: yup.string().required("Username obrigatório"),
  password: yup.string().required("Senha obrigatória"),
});

export const LoginSide = () => {
  const router = useRouter();
  const { handleLogin } = useAuth();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginForm>({
    resolver: yupResolver(loginSchema),
  });

  const onSubmit: SubmitHandler<LoginForm> = async (data) => {
    try {
      const isLoginSuccessful = await handleLogin(data);
      const isAuth = await isAuthenticated();

      if (isLoginSuccessful && isAuth) {
        toast.success("Login efetuado com sucesso!");
        router.push(APP_ROUTES.private.refund);
      } else {
        toast.error("Usuário e/ou senha incorretos");
      }
    } catch (error) {
      console.error("error:", error);
      toast.error("Houve um erro ao realizar o login");
    }
  };

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className="flex max-w-96 flex-col items-center justify-between gap-10 p-4 md:w-96"
    >
      <div className="flex flex-col items-center justify-center gap-2">
        <Image
          src="/logo.png"
          alt="logo"
          width={200}
          height={200}
          className="h-40 w-40 object-contain"
        />
      </div>
      <Typography align="center" variant="body1">
        Bem-vindo de volta! Por favor, faça login para continuar de onde parou.
      </Typography>
      <div className="flex w-full flex-col gap-8">
        <TextField
          id="input-with-icon-textfield"
          label="Username"
          placeholder="Insira o username"
          variant="outlined"
          {...register("username")}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <AccountCircle />
              </InputAdornment>
            ),
          }}
          error={!!errors?.username?.message}
          helperText={errors?.username?.message}
        />
        <TextField
          type="password"
          id="input-with-icon-textfield"
          label="Senha"
          placeholder="Insira a senha"
          variant="outlined"
          {...register("password")}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <LockRounded />
              </InputAdornment>
            ),
          }}
          error={!!errors?.password?.message}
          helperText={errors?.password?.message}
        />
      </div>

      <Button type="submit" variant="contained" className="w-full">
        Entrar
      </Button>
    </form>
  );
};
