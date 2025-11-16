import { BaseEntity } from "./BaseEntity";

export interface RoomLocation extends BaseEntity {
 name: string;
 description?: string;
 building?: string;
}

