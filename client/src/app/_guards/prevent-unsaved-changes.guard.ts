import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { inject } from '@angular/core';
import { ConfirmService } from '../_services/confirm.service';

//This Guard is to prevent users from leaving the page if the form they are busy with is dirty
export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
  const confirmService = inject(ConfirmService)

  if(component.editForm?.dirty){
    return confirmService.confirm();
  }
  return true;
};
