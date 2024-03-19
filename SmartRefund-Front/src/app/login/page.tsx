import { BannerSide } from "@/components/loginPage/BannerSide";
import { LoginSide } from "@/components/loginPage/LoginSide";

export default function Login() {
  return (
    <main className="login-grid-container">
      <BannerSide />
      <LoginSide />
    </main>
  );
}
