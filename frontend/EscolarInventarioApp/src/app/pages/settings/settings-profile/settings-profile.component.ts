import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { School } from '../../../core/models/School';
import { UpdateSchoolRequest } from '../../../core/models/UpdateSchoolRequest';
import { SchoolService } from '../../../core/Services/school.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-settings-profile',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './settings-profile.component.html',
  styleUrls: ['./settings-profile.component.scss']
})
export class SettingsProfileComponent implements OnInit {
  form!: FormGroup;
  schoolId!: number;
  school = {} as School;

  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private schoolService: SchoolService,
    private toastr: ToastrService,
    private location: Location,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadSchool();
  }

  initForm(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      inep: [''],
      address: [''],
      city: ['']
    });
  }

  private loadSchool(): void {
    this.schoolService.getSchoolData().subscribe({
      next: (school) => {
        this.school = school;
        this.schoolId = school.id;

        this.form.patchValue({
          name: school.name,
          inep: school.inep,
          address: school.address,
          city: school.city
        });
      },
      error: () => {
        this.toastr.error('Erro ao carregar dados de perfil', 'Erro');
      }
    });
  }

  saveSchool(): void {
    if (this.form.invalid) return;

    const request: UpdateSchoolRequest = this.form.value;

    this.schoolService.updateSchool(this.schoolId, request).subscribe({
      next: () => this.toastr.success('Perfil atualizado!', 'Sucesso'),
      error: () => this.toastr.error('Erro ao atualizar perfil.', 'Erro')
    });
  }

  resetForm(): void {
    this.form.reset();
    this.loadSchool();
  }

  goBack(): void {
    this.location.back();
  }

  cssValidator(campoForm: FormControl | AbstractControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }
}
