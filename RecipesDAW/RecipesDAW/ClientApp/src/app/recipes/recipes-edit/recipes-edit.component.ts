import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, Form } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { RecipesService } from '../recipes.service';
import { Recipe, Ingredient, Instruction } from './../recipes.models';

@Component({
  selector: 'app-recipes-edit',
  templateUrl: './recipes-edit.component.html',
  styleUrls: ['./recipes-edit.component.css']
})
export class RecipesEditComponent implements OnInit {

  private routerLink: string = '../list';

  public recipe: Recipe;

  private recipeID: string;

  private isEdit: boolean = false;

  public formGroup: FormGroup;
  private instructions: FormArray;
  private ingredients: FormArray;

  private errorMessage: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private recipesService: RecipesService,
    private formBuilder: FormBuilder) { }



  ngOnInit() {

    this.recipeID = this.route.snapshot.params['id'];

    if (this.recipeID != 'new') {
      this.routerLink = '../../list';
      this.isEdit = true;
    }
    else {
      this.recipesService.loadRecipe(this.recipeID).subscribe(res => {
        this.recipe = res;
      });
    }

    this.initForm(this.recipe ? this.recipe : <Recipe>{});

  }



  save() {
    Object.keys(this.formGroup.controls).forEach(control => {
      this.formGroup.get(control).markAsTouched();
    });
    const { title, description, serves, imageUrl, ingredients, instructions } = this.formGroup.value;

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
      this.formGroup = this.formBuilder.group({
        title: [recipe.title],
        description: [recipe.description],
        serves: [recipe.serves],
        imageUrl: [recipe.imageUrl],
        ingredients: this.formBuilder.array([]),
        instructions: this.formBuilder.array([])
      });
    }
    else {
      this.formGroup = this.formBuilder.group({
        title: [''],
        description: [''],
        serves: [''],
        imageUrl: [''],
        instructions: this.formBuilder.array([]),
        ingredients: this.formBuilder.array([])
      });
    }

    this.instructions = this.formGroup.get('instructions') as FormArray;
    this.ingredients = this.formGroup.get('ingredients') as FormArray;

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
  }



  private createIngredient(amount: string, name: string): FormGroup {
    return this.formBuilder.group({
      amount: [amount],
      name: [name]
    });
  }



  private createInstruction(instruction: string): FormGroup {
    return this.formBuilder.group({
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
