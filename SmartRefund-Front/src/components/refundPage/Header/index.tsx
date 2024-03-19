import useAuth from "@/hooks/useAuth";
import { APP_ROUTES } from "@/utils/constants/app-routes";
import { AppBar, Box, Button, Toolbar, Typography } from "@mui/material";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { toast } from "react-toastify";

export const Header = () => {
  const router = useRouter();
  const { handleLogout } = useAuth();

  const logoutHandler = () => {
    handleLogout();
    toast.success("Logout efetuado com sucesso!");
    router.push(APP_ROUTES.public.login);
  };

  return (
    <Box
      sx={{
        flexGrow: 1,
        position: "fixed",
        width: "100%",
        zIndex: 1000,
        gridArea: "header",
      }}
      className="z-10 sticky"
    >
      <AppBar position="static" sx={{ bgcolor: "white" }}>
        <Toolbar>
          <Image
            src="/logo_without_name.png"
            alt="logo"
            width={200}
            height={200}
            className="w-12 h-12 object-cover"
          />
          <Typography
            variant="h6"
            component="div"
            sx={{ flexGrow: 1, paddingLeft: 4 }}
            color="primary"
          >
            Smart Refund
          </Typography>
          <Button variant="contained" onClick={() => logoutHandler()}>
            Sair
          </Button>
        </Toolbar>
      </AppBar>
    </Box>
  );
};
