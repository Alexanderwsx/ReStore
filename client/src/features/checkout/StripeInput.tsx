import { InputBaseComponentProps } from "@mui/material";
import { Ref, forwardRef, useImperativeHandle, useRef } from "react";

//material ui accepte pas les props de stripe donc on doit faire un wrapper
//normalment on aurait pas besoin mais on a besoin pour materialui
interface Props extends InputBaseComponentProps {}
export const StripeInput = forwardRef(function StripeInput(
  { component: Component, ...props }: Props,
  ref: Ref<unknown>
) {
  const elementRef = useRef<any>();

  useImperativeHandle(ref, () => ({
    focus: () => elementRef.current.focus,
  }));
  return (
    <Component
      onReady={(element: any) => (elementRef.current = element)}
      {...props}
    />
  );
});
