import { BaseEntity } from "./BaseEntity";
import { Category } from './Category';
import { ConservationState } from "./enum/ConservationState.enum";
import { RoomLocation } from './RoomLocation';

export interface Asset extends BaseEntity {
  name: string;
  description?: string;
  patrimonyCode?: number;
  acquisitionValue?: number;
  conservationState: ConservationState;
  serieNumber?: string;
  category: Category;
  roomLocation?: RoomLocation;
}
