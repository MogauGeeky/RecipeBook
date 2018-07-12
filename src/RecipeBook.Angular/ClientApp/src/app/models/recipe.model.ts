import { RecipeStep } from './recipe-step.model';

export class RecipeEntry {
    public id: string;
    public title: string;
    public description: string;
    public notes: string;
    public ownerId: string;
    public recipeEntrySteps: RecipeStep[];
}
