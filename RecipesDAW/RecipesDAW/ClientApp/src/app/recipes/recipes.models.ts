export interface Ingredient {
  id: string;
  amount: string;
  name: string;
}

export interface Instruction {
  id: string;
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
