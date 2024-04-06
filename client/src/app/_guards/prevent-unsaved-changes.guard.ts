import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

//This Guard is to prevent users from leaving the page if the form they are busy with is dirty
export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
  if(component.editForm?.dirty){
    return confirm('Are you sure you want to continue? Any unsaved chnages will be lost')
  }
  return true;
};
