export const InternalReceiptStatusEnum = {
  0: { label: "Não definido", color: "error" },
  1: { label: "Não processado", color: "info" },
  2: { label: "Sucesso", color: "primary" },
  3: { label: "Falhou uma vez", color: "warning" },
  4: { label: "Falhou mais de uma vez", color: "warning" },
  5: { label: "Sem sucesso", color: "error" },
  6: { label: "Falha na autenticação do Vision", color: "error" },
} as const;

export const TranslatedVisionReceiptCategoryEnum = {
  0: { label: "Não definido", color: "error" },
  1: { label: "Hospedagem", color: "info" },
  2: { label: "Transporte", color: "primary" },
  3: { label: "Viagem", color: "warning" },
  4: { label: "Alimentação", color: "primary" },
  5: { label: "Outros", color: "error" },
} as const;

export const StatusRefundEnum = {
  0: { label: "Não definido", color: "error" },
  1: { label: "Submetido", color: "info" },
  2: { label: "Pago", color: "primary" },
  3: { label: "Recusado", color: "error" },
} as const;
