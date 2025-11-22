import Image from "next/image";
import NavBar from "./nav/NavBar";
import Listings from "./Auctions/Listings";
import { Console } from "console";

export default function Home() {
  console.log("Server Component");
  return (
   <>
   <div>
    <h3>
      Carsties App!</h3>
   </div>
   <div>
    <Listings/>
   </div>
   </>
  );
}
