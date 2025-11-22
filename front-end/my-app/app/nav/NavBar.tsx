import React from 'react'
import { AiOutlineCar } from 'react-icons/ai'

export default function NavBar() {
  return (
    <header className='sticky top-0 z-50 flex justify-between bg-white p-5
     text-gray-800 items-center shadow-md'>
        <div className='items-center flex text-red-500 gap-2 text-3xl font-semibold'>
        <AiOutlineCar size={50}/>
        <div>Carsties Auctions</div>
        </div>
        <div className='items-center flex text-red-500 gap-2 text-2xl font-semibold'>search</div>
        <div className='items-center flex text-red-500 gap-2 text-2xl font-semibold'>login</div>
    </header>
  )
}
