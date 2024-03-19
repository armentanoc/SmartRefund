import { SvgIconComponent } from "@mui/icons-material";
import { Chip, Typography } from "@mui/material";

interface InfoSectionProps {
  icon: React.ReactElement<SvgIconComponent>;
  label: string;
  value?: string | number;
  chip?: {
    label: string;
    color:
      | "success"
      | "info"
      | "warning"
      | "error"
      | "primary"
      | "secondary"
      | "default";
  };
}

export const InfoSection = ({ icon, label, value, chip }: InfoSectionProps) => {
  return (
    <div className="flex justify-between items-start gap-4">
      <Typography
        variant="body1"
        color="text.secondary"
        className="flex items-center gap-2"
      >
        {icon}
        {label}
      </Typography>
      {value && (
        <Typography
          variant="body1"
          className="flex items-center gap-2"
          align="right"
        >
          {value}
        </Typography>
      )}
      {chip && <Chip label={chip.label} color={chip.color} />}
    </div>
  );
};
