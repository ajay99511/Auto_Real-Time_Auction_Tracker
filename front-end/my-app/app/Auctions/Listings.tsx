import React from 'react'
import AuctionCard from './AuctionCard';
import { Auction, PagedResult } from '../types';
import AppPagination from '../components/AppPagination';

async function getData():Promise<PagedResult<Auction>> {
    const res = await fetch('http://localhost:6001/search');
    if(!res.ok) throw new Error("Failed to fetch data");
    const data = res.json();
    return data;
}

export default async function Listings() {
    const data = await getData();
    // console.log(data.pageCount);
    // console.log(data.totalcount);
  return (
    <>
    {/* <div>
        {JSON.stringify(data,null,2)}
    </div> */}
    <div className='grid grid-cols-4 gap-6'>
      {data && data.results.map((auction)=>(
          <AuctionCard key={auction.id}  auction={auction}/>
        ))}
    </div>
    <div className='flex justify-center mt-4'>
      <AppPagination currentPage={data.pageCount} totalPages={data.totalcount}/>
    </div>
    </>
  )
}
