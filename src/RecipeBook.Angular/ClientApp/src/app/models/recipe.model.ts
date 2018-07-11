import { RecipeStep } from './recipe-step.model';

export class RecipeEntry {
    public id: string;
    public title: string;
    public description: string;
    public notes: string;
    public ownerid: string;
    public recipeEntrySteps: RecipeStep[];
}
