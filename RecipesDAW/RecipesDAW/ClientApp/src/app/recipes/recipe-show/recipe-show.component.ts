import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { RecipesService } from '../recipes.service';
import { Recipe, Ingredient, Instruction } from './../recipes.models';

@Component({
  selector: 'app-recipe-show',
  templateUrl: './recipe-show.component.html',
  styleUrls: ['./recipe-show.component.css']
})
export class RecipeShowComponent implements OnInit {

  public recipe: Recipe;

  public recipes: Recipe[];

  private recipeID: string;

  private routerLink: string = '../edit';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private recipesService: RecipesService) { }

  ngOnInit() {
    this.recipeID = this.route.snapshot.params['id'];

    if (this.recipeID) {
      this.routerLink = '../../list';
    }

    this.recipesService.loadRecipe(this.recipeID).subscribe(res => {
      this.recipe = res;
    });
  }

  deleteRecipe(recipe: Recipe) {
    this.recipesService.deleteRecipe(recipe.id).subscribe(x => {
      this.loadRecipes();
    });
    this.router.navigate(['']);

  }

  loadRecipes() {
    this.recipesService.listRecipes().subscribe(res => {
      this.recipes = res;
    })
  }
}
