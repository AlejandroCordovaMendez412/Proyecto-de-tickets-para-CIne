import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { AsignacionesRoutingModule } from './asignaciones-routing.module';
import { AsignacionesComponent } from './asignaciones.component';
@NgModule({ declarations: [AsignacionesComponent], imports: [SharedModule, AsignacionesRoutingModule] })
export class AsignacionesModule {}
