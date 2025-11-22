export type PagedResult<T>=
{
    results:T[]
    pageCount : number
    totalcount : number
}
export type Auction =
{
  id: string
  reservePrice: number
  seller: string
  winner: any
  soldAmount: any
  currentHighBid: any
  createdAt: string
  updatedAt: string
  auctionEnd: string
  status: string
  make: string
  model: string
  year: number
  color: string
  mileage: number
  imageUrl: string
}