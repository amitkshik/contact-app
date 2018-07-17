import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';

import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, AUTOCOMPLETE_PANEL_HEIGHT } from '@angular/material';

import { ContactlistComponent } from '../contactlist/contactlist.component';

import { IContact } from '../model/contact';
import { ContactService } from '../services/contact.service';
import { DBOperation } from '../shared/DBOperation';
import { Global } from '../shared/Global';


export  enum personTypes {
  Supplier = 1,
  Customer = 0

}

@Component({
  selector: 'app-contactform',
  templateUrl: './contactform.component.html',
  styleUrls: ['./contactform.component.css']
})

export class ContactformComponent implements OnInit {
  msg: string;
  indLoading = false;
  contactFrm: FormGroup;
  // dbops: DBOperation;
  // modalTitle: string;
  // modalBtnTitle: string;
  listFilter: string;
  selectedOption: string;
  // contact: IContact;
  personTypes: typeof personTypes = personTypes;
  personType = [];

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private _contactService: ContactService,
    public dialogRef: MatDialogRef<ContactlistComponent>) { }

  ngOnInit() {
    // built contact form
    this.contactFrm = this.fb.group({
      id: [''],
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      telephone: ['', [Validators.required]],
      birthday: ['', [Validators.required]],
      personTypeId: ['', [Validators.required]]
    });

    this.personType = Global.PersonType;

    // subscribe on value changed event of form to show validation message
    this.contactFrm.valueChanges.subscribe(data => this.onValueChanged(data));
    this.onValueChanged();

    if (this.data.dbops === DBOperation.create) {
      this.contactFrm.reset();
    } else {
      this.contactFrm.setValue(this.data.contact);
    }
    this.SetControlsState(this.data.dbops === DBOperation.delete ? false : true);
  }
  // form value change event
  onValueChanged(data?: any) {
    if (!this.contactFrm) { return; }
    const form = this.contactFrm;
    // tslint:disable-next-line:forin
    for (const field in this.formErrors) {
      // clear previous error message (if any)
      this.formErrors[field] = '';
      const control = form.get(field);
      // setup custom validation message to form
      if (control && control.dirty && !control.valid) {
        const messages = this.validationMessages[field];
        // tslint:disable-next-line:forin
        for (const key in control.errors) {
          this.formErrors[field] += messages[key] + ' ';
        }
      }
    }
  }
  // form errors model
  // tslint:disable-next-line:member-ordering
  formErrors = {
    'firstname': '',
    'lastname': '',
    'email': '',
    'personType': '',
    'birth': '',
    'telephone': ''
  };
  // custom valdiation messages
  // tslint:disable-next-line:member-ordering
  validationMessages = {
    'firstname': {
      'maxlength': 'Name cannot be more than 50 characters long.',
      'required': 'Name is required.'
    },
    'lastname': {
      'maxlength': 'Name cannot be more than 50 characters long.',
      'required': 'Name is required.'
    },
    'email': {
      'email': 'Invalid email format.',
      'required': 'Email is required.'
    },
    'personTypeId': {
      'required': 'personType is required.'
    },
    'telephone': {
      'required': 'telephone is required.'
    },
    'birth': {
      'required': 'Birthday is required.'
    }

  };
  onSubmit(formData: any) {
    const contactData = this.mapDateData(formData.value);
    contactData.personTypeId = Global.PersonType.findIndex(t => t === contactData.personTypeId.toString());
    switch (this.data.dbops) {
      case DBOperation.create:
        this._contactService.addContact(Global.BASE_USER_ENDPOINT + 'addContact', contactData).subscribe(
          data => {
            // Success
            if (data.message) {
              this.dialogRef.close('success');
            } else {
              this.dialogRef.close('error');
            }
          },
          error => {
            this.dialogRef.close('error');
          }
        );
        break;
      case DBOperation.update:
        this._contactService.updateContact(Global.BASE_USER_ENDPOINT + 'updateContact', contactData.id, contactData).subscribe(
          data => {
            // Success
            if (data.message) {
              this.dialogRef.close('success');
            } else {
              this.dialogRef.close('error');
            }
          },
          error => {
            this.dialogRef.close('error');
          }
        );
        break;
      case DBOperation.delete:
        this._contactService.deleteContact(Global.BASE_USER_ENDPOINT + 'deleteContact', contactData.id).subscribe(
          data => {
            // Success
            if (data.message) {
              this.dialogRef.close('success');
            } else {
              this.dialogRef.close('error');
            }
          },
          error => {
            this.dialogRef.close('error');
          }
        );
        break;
    }
  }
  SetControlsState(isEnable: boolean) {
    isEnable ? this.contactFrm.enable() : this.contactFrm.disable();
  }

  mapDateData(contact: IContact): IContact {
    contact.birthday = new Date(contact.birthday).toISOString();
    return contact;
  }

}
