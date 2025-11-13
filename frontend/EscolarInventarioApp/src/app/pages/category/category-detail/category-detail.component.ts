import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Category } from '../../../core/models/Category';
import { CategoryService } from '../../../core/Services/category.service';
import { Location } from '@angular/common';


@Component({
  selector: 'app-category-detail',
  imports: [ReactiveFormsModule, CommonModule, NgxSpinnerModule],
  templateUrl: './category-detail.component.html',
  styleUrl: './category-detail.component.scss'
})
export class CategoryDetailComponent implements OnInit {

  form!: FormGroup;
  category = {} as Category;
  isEditMode = false;


  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.validation();

    const categoryIdParam = this.route.snapshot.paramMap.get('id');

    if (categoryIdParam) {
      this.isEditMode = true;
      this.getCategoryById(+categoryIdParam);
    }
  }

  validation(): void {
   this.form = this.fb.group({
  name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
  description: ['', Validators.maxLength(200)],
});
  }

saveCategory(): void {
  if (this.form.invalid) return;

  const categoryData = {
    ...this.form.value,
    id: this.isEditMode ? this.category.id : undefined
  };

  const request = this.isEditMode
    ? this.categoryService.updateCategory(categoryData)
    : this.categoryService.insertCategory(categoryData);

  request.subscribe({
    next: () => {
      this.toastr.success('Categoria salva com sucesso!', 'Sucesso');
      this.router.navigate(['/category']);
    },
    error: () => this.toastr.error('Erro ao salvar categoria', 'Erro')
  });
  }

getCategoryById(id: number): void {
  this.categoryService.getCategoryById(id).subscribe({
    next: (category: Category) => {
      this.category = category;
      this.form.patchValue({
        name: category.name,
        description: category.description
      });
    },
    error: () => this.toastr.error('Erro ao carregar a Categoria.', 'Erro')
  });
  }


  resetForm(): void {
    this.form.reset();
  }

  goBack(): void {
    this.location.back();
  }

  cssValidator(campoForm: FormControl | AbstractControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }
}
