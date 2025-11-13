import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { RoomLocation } from '../../../core/models/RoomLocation';
import { RoomLocationService } from '../../../core/Services/room-location.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-room-location-detail',
  imports: [ReactiveFormsModule, CommonModule, NgxSpinnerModule],
  templateUrl: './room-location-detail.component.html',
  styleUrl: './room-location-detail.component.scss'
})
export class RoomLocationDetailComponent implements OnInit {

  form!: FormGroup;
  roomLocation = {} as RoomLocation;
  isEditMode = false;



  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private roomLocationService: RoomLocationService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.validation();

    const roomLocationIdParam = this.route.snapshot.paramMap.get('id');

    if (roomLocationIdParam) {
      this.isEditMode = true;
      this.getRoomLocationById(+roomLocationIdParam);
    }
  }

  validation(): void {
   this.form = this.fb.group({
  name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
  description: ['', Validators.maxLength(200)],
  building: ['',Validators.maxLength(100)],
});
  }


saveRoomLocation(): void {
  if (this.form.invalid) return;

  const roomLocationData = {
    ...this.form.value,
    id: this.isEditMode ? this.roomLocation.id : undefined
  };

  const request = this.isEditMode
    ? this.roomLocationService.updateRoomLocation(roomLocationData)
    : this.roomLocationService.insertRoomLocation(roomLocationData);

  request.subscribe({
    next: () => {
      this.toastr.success('Localização salva com sucesso!', 'Sucesso');
      this.router.navigate(['/roomlocation']);
    },
    error: (error) => this.toastr.error('Erro ao salvar localização', error)
  });
  }

getRoomLocationById(id: number): void {
  this.roomLocationService.getRoomLocationById(id).subscribe({
    next: (roomLocation: RoomLocation) => {
      this.roomLocation = roomLocation;
      this.form.patchValue({
        name: roomLocation.name,
        description: roomLocation.description,
        building: roomLocation.building
      });
    },
    error: (error) => this.toastr.error('Erro ao carregar a localização.', error)
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

