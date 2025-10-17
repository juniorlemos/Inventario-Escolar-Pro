import { Asset } from "./Asset";
import { BaseEntity } from "./BaseEntity";
import { RoomLocation } from "./RoomLocation";

export interface AssetMovement extends BaseEntity {
  asset?: Asset;
  fromRoom?: RoomLocation;
  toRoom?: RoomLocation;
  movedAt: Date;
  responsible?: string;
  isCanceled: boolean;
  canceledAt?: Date
  cancelReason?: string;
}
