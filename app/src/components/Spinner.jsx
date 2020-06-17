import React from "react";
import { FadeLoader } from "react-spinners";

const Spinner = ({ isLoading }) => {
  if (isLoading) {
    return (
      <div className="loader">
        <div className="loader-content">
          <FadeLoader />
        </div>
        <div className="loader-message">Loading</div>
      </div>
    );
  } else {
    return <div />;
  }
};

export default Spinner;
