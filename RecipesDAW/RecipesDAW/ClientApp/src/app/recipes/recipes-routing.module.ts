import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RecipesComponent } from './recipes.component';

import { RecipesListComponent } from './recipes-list/recipes-list.component';
import { RecipesEditComponent } from './recipes-edit/recipes-edit.component';
import { componentFactoryName } from '@angular/compiler';
import { RecipeShowComponent } from './recipe-show/recipe-show.component';

const routes: Routes = [
  {
    path: '', component: RecipesComponent, data: { navArea: 'duckbill' },
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: RecipesListComponent },
      { path: 'edit/:id', component: RecipesEditComponent },
      { path: 'edit', component: RecipesEditComponent },
      { path: 'show/:id', component: RecipeShowComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RecipesRoutingModule {
  static routedComponents = [RecipesComponent, RecipesListComponent, RecipesEditComponent];
}
