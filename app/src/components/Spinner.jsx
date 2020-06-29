import React from "react";

const Spinner = ({ isLoading }) => {
  if (isLoading) {
    return (
      <div id="loader" className="loader">
        <div className="loader-content">
         
        </div>
      </div>
    );
  } else {
    return <div />;
  }
};

export default Spinner;
