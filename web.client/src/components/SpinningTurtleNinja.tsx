import "./SpinningTurtleNinja.css";

export default function SpinningTurtleNinja() {
  return (
    <div className="stn-wrapper">
      <div className="turtle-container">
        <div className="turtle">
          <div className="shell" />
          <div className="head">
            <div className="mask" />
            <div className="eye left" />
            <div className="eye right" />
          </div>
          <div className="turtle-label">cipas</div>
        </div>
      </div>
    </div>
  );
}
