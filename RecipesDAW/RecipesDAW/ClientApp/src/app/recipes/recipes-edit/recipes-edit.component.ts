import { Component, OnInit, PACKAGE_ROOT_URL } from '@angular/core';
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

  private emptyIp: string = '00000000-0000-0000-0000-000000000000';

  private isEdit: boolean = false;

  public message: string;

  public recipeForm: FormGroup;
  private instructions: FormArray;
  private ingredients: FormArray;

  private errorMessage: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private recipesService: RecipesService,
    private fb: FormBuilder
  ) {
  }



  ngOnInit() {
   
      this.recipeID = this.route.snapshot.params['id'];

      if (this.recipeID != 'new') {
        this.routerLink = '';
        this.isEdit = true;
      }
      else {
        this.recipeID = null;
      }

      this.recipesService.loadRecipe(this.recipeID).subscribe(res => {
        this.recipe = res;
        this.initForm(this.recipe);
      });
  }



  save() {
    if (this.recipeForm.valid) {
      Object.keys(this.recipeForm.controls).forEach(control => {
        this.recipeForm.get(control).markAsTouched();
      });
      const { title, description, serves, imageUrl, ingredients, instructions } = this.recipeForm.value;

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
        instructions
      };
      this.recipesService.saveRecipe(recipe).subscribe(res => {
        this.router.navigate(['']);
      });
    }
    else {
      this.message = "All tabs are required";
    }
  }



  initForm(recipe: Recipe) {

    if (this.isEdit) {
      this.recipeForm = this.fb.group({
        title: [recipe.title, [Validators.required]],
        description: [recipe.description, [Validators.required]],
        serves: [recipe.serves, [Validators.required]],
        imageUrl: [recipe.imageUrl, [Validators.required]],
        instructions: this.fb.array([]),
        ingredients: this.fb.array([])
      });
    }
    else {
      this.recipeForm = this.fb.group({
        title: ['', [Validators.required]],
        description: ['',[ Validators.required]],
        serves: ['', [Validators.required]],
        imageUrl: ['', [Validators.required]],
        instructions: this.fb.array([]),
        ingredients: this.fb.array([])
      });
    }

    this.instructions = this.recipeForm.get('instructions') as FormArray;
    this.ingredients = this.recipeForm.get('ingredients') as FormArray;

    if (this.isEdit) {
      this.recipe.ingredients.forEach(ingredient => {
        this.ingredients.push(this.createIngredient(ingredient.id, ingredient.amount, ingredient.name));
      });

      this.recipe.instructions.forEach(instruction => {
        this.instructions.push(this.createInstruction(instruction.id, instruction.instruction));
      });
    }
    else {
      this.instructions.push(this.createInstruction(this.emptyIp, ""));
      this.ingredients.push(this.createIngredient(this.emptyIp,"", ""));
    }
  }



  private createIngredient(id: string, amount: string, name: string): FormGroup {
    return this.fb.group({
      id: [id],
      amount: [amount, [Validators.required]],
      name: [name, [Validators.required]]
    });
  }



  private createInstruction(id: string, instruction: string): FormGroup {
    return this.fb.group({
      id: [id],
      instruction: [instruction, [Validators.required]]
    });
  }


  addIngredient(): void {
    this.ingredients.push(this.createIngredient(this.emptyIp,'', ''));
  }



  addInstruction(): void {
    this.instructions.push(this.createInstruction(this.emptyIp,''));
  }



  deleteIngredient(index: number): void {
    this.ingredients.removeAt(index);
  }


  deleteInstruction(index: number): void {
    this.instructions.removeAt(index);
  }
}
