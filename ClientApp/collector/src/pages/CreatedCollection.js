import { Link } from "react-router-dom";

const CreatedCollection = () => {
    return (
        <>
            <p>Collection created</p>
            <Link to='/collections'>Back to my collections</Link>
        </>
    );
}

export default CreatedCollection;