import { Brand } from "./brand";
import { Variant } from "./variant";

export class Product {
  id!: number;
  name!: string;
  type!: ProductType;
  brandId!: number;
  brand!: Brand;
  variants!: Variant[];
}
export enum ProductType {
  Mug = 'Mug',
  Jug = 'Jug',
  Cup = 'Cup'
}
export enum Size {
  Small = 'Small',
  Medium = 'Medium',
  Large = 'Large'
}
