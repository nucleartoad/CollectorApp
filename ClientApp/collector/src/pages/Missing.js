import { Link } from "react-router-dom";

const Missing = () => {
    return (
        <>
            <h2>page could not be found</h2>
            <br /><Link to="/">Click to go back Home</Link><br />
        </>
    );
};

export default Missing;