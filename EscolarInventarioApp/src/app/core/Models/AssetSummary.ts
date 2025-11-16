export interface AssetSummary {
  id: number;
  name: string;
  roomLocation?: {
    id: number;
    name: string;
  };
}
