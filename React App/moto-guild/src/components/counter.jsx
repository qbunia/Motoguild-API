import React, { Component } from "react";

class Counter extends Component {
  state = {
    count: 0,
  };

  constructor() {
    super();
    this.handleIncrement = this.handleIncrement.bind(this);
  }

  product = {
    id: 1,
    name: "Buty",
  };

  handleIncrement = (product) => {
    console.log(product);
    this.setState({ count: this.state.count + 1 });
  };

  render() {
    return (
      <>
        <span className={this.getBadgeClasses()}>{this.formatCount()}</span>
        <button
          onClick={() => this.handleIncrement(this.product)}
          className="btn btn-secondary btn-sm"
        >
          Increment
        </button>
      </>
    );
  }

  getBadgeClasses() {
    let classes = "badge m-2 bg-";
    classes += this.state.count === 0 ? "warning" : "primary";
    return classes;
  }

  formatCount() {
    return this.state.count === 0 ? "Zero" : this.state.count;
  }
}

export default Counter;
