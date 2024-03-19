import ApplicationContainer from "@/components/Auth/ApplicationContainer";
import { CssBaseline } from "@mui/material";
import type { Metadata } from "next";
import { Roboto } from "next/font/google";
import "./globals.css";

const roboto = Roboto({
  weight: ["300", "400", "500"],
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "Smart Refund",
  description:
    "Projeto de processamento de imagens de notas fiscais para automatização de reembolsos corporativos",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="pt-br">
      <CssBaseline />
      <body
        className={roboto.className}
        style={{ minHeight: "100vh", height: "100vh" }}
      >
        <ApplicationContainer>{children}</ApplicationContainer>
      </body>
    </html>
  );
}
