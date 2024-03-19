export const filters: Record<FilterOptionKey, FilterOption> = {
  optionsStatusGPT: {
    label: "Status de Envio para o GPT",
    key: "optionsStatusGPT",
    options: [
      {
        label: "Não processado",
        value: "1",
      },
      {
        label: "Sucesso",
        value: "2",
      },
      {
        label: "Falhou uma vez",
        value: "3",
      },
      {
        label: "Falhou duas vezes",
        value: "4",
      },
      {
        label: "Sem sucesso",
        value: "5",
      },
      {
        label: "Falha na autenticação do Vision",
        value: "6",
      },
    ],
  },

  optionsStatusTranslate: {
    label: "Status da Tradução",
    key: "optionsStatusTranslate",
    options: [
      {
        label: "Tradução bem sucedida",
        value: "1",
      },
      {
        label: "Erro da tradução",
        value: "2",
      },
    ],
  },

  optionsStatusRefund: {
    label: "Status do Reembolso",
    key: "optionsStatusRefund",
    options: [
      {
        label: "Submetido",
        value: "1",
      },
      {
        label: "Pago",
        value: "2",
      },
      {
        label: "Recusado",
        value: "3",
      },
    ],
  },
};

type FilterOptionKey =
  | "optionsStatusGPT"
  | "optionsStatusTranslate"
  | "optionsStatusRefund";

export type optionsType = {
  label: string;
  value: string;
};

export type FilterOption = {
  label: string;
  key: FilterOptionKey;
  options: optionsType[];
};
