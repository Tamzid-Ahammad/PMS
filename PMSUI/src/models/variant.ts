import { Size } from "./product";

export class Variant {
  id!: number;
  color!: string;
  specification!: string;
  size!: Size;
  price!: number;
}
