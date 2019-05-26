import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, Form, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { RecipesService } from '../recipes.service';
import { Recipe, Ingredient, Instruction } from './../recipes.models';

@Component({
  selector: 'app-recipes-edit',
  templateUrl: './recipes-edit.component.html',
  styleUrls: ['./recipes-edit.component.css']
})
export class RecipesEditComponent implements OnInit {

  private routerLink: string = '';

  public recipe: Recipe;

  private recipeID: string;

  private isEdit: boolean = false;

  public recipeForm: FormGroup;
  private instructions: FormArray;
  private ingredients: FormArray;

  private errorMessage: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private recipesService: RecipesService,
    private fb: FormBuilder) { }



  ngOnInit() {

    this.recipeID = this.route.snapshot.params['id'];

    if (this.recipeID != 'new') {
      this.routerLink = '';
      this.isEdit = true;
    }
    else {
      this.recipeID = null;
      //this.recipe = <Recipe>{};
    }

    this.recipesService.loadRecipe(this.recipeID).subscribe(res => {
      this.recipe = res;
      this.initForm(this.recipe);
    });

  }



  save() {
    Object.keys(this.recipeForm.controls).forEach(control => {
      this.recipeForm.get(control).markAsTouched();
    });
    const { title, description, serves, imageUrl, ingredients, instructions } = this.recipeForm.value;

    //const filteredInstructions = instructions.map(item => item.instruction);

    let id;

    if (this.isEdit) {
      id = this.recipeID;
    }

    let recipe: Recipe = {
      id: id,
      title,
      description,
      serves,
      imageUrl,
      ingredients,
      //instructions: filteredInstructions
      instructions
    };
      this.recipesService.saveRecipe(recipe).subscribe(res => {
        this.router.navigate(['']);
      });
  }



  initForm(recipe: Recipe) {

    if (this.isEdit) {
      this.recipeForm = this.fb.group({
        title: [recipe.title],
        description: [recipe.description],
        serves: [recipe.serves],
        imageUrl: [recipe.imageUrl],
        instructions: this.fb.array([]),
        ingredients: this.fb.array([])
      });
    }
    else {
      this.recipeForm = this.fb.group({
        title: [''],
        description: [''],
        serves: [''],
        imageUrl: [''],
        instructions: this.fb.array([]),
        ingredients: this.fb.array([])
      });
    }

    this.instructions = this.recipeForm.get('instructions') as FormArray;
    this.ingredients = this.recipeForm.get('ingredients') as FormArray;

    if (this.isEdit) {
      this.recipe.ingredients.forEach(ingredient => {
        this.ingredients.push(this.createIngredient(ingredient.amount, ingredient.name));
      });

      this.recipe.instructions.forEach(instruction => {
        this.instructions.push(this.createInstruction(instruction.instruction));
      });
    }
    else {
      this.instructions.push(this.createInstruction(""));
      this.ingredients.push(this.createIngredient("", ""));
    }
    //console.log(this.recipeForm.value);
  }



  private createIngredient(amount: string, name: string): FormGroup {
    return this.fb.group({
      amount: [amount],
      name: [name]
    });
  }



  private createInstruction(instruction: string): FormGroup {
    return this.fb.group({
      instruction: [instruction]
    });
  }


  addIngredient(): void {
    this.ingredients.push(this.createIngredient('', ''));
  }



  addInstruction(): void {
    this.instructions.push(this.createInstruction(''));
  }



  deleteIngredient(index: number): void {
    this.ingredients.removeAt(index);
  }


  deleteInstruction(index: number): void {
    this.instructions.removeAt(index);
  }
}
