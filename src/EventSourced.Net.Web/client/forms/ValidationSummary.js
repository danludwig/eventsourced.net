import React, { Component, PropTypes } from 'react'
import { connect } from 'react-redux'
import Markdown from 'react-remarkable'

class ValidationSummary extends Component {

  summarize() {
    const { form } = this.props, errors = []
    if (form.submitting) return false
    if (form.error) errors.push(form.error)
    for (var field in form.errors) {
      if (!form.errors.hasOwnProperty(field)) continue
      if (!form.errors[field]) continue
      errors.push(form.errors[field])
    }
    if (!errors.length) return false
    return errors
  }

  render() {
    const errors = this.summarize()
    return errors && errors.length ? this.renderSummary(errors) : false
  }

  renderSummary(errors) {
    return(
      <div className="text-danger form-errors">
        <ul>
          {errors.map((entry, i) =>
            <li key={i}>
              <Markdown>{entry}</Markdown>
            </li>
          )}
        </ul>
      </div>
    )
  }

  static get propTypes() {
    return {
      form: PropTypes.object.isRequired
    }
  }
}

function select(state) {
  // validation summary state selector
  return { }
}

export default connect(select)(ValidationSummary)
