export interface Ingredient {
  amount: string;
  name: string;
}

export interface Instruction {
  instruction: string;
}

export interface Recipe {
  id: string;
  title: string;
  description: string;
  serves: string;
  imageUrl: string;
  ingredients: Ingredient[];
  instructions: Instruction[];
}
