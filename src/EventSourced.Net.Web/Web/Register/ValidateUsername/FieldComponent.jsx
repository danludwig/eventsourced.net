import React, { Component, PropTypes } from 'react'
import validate, { messages } from './validation'
import Markdown from 'react-remarkable'

class Field extends Component {
  static propTypes = {
    className: PropTypes.string.isRequired,
    asyncValidating: PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.bool
    ]).isRequired,
    submitting: PropTypes.bool.isRequired,
    hasError: PropTypes.bool.isRequired,
    hasSuccess: PropTypes.bool.isRequired,
    field: PropTypes.object.isRequired,
    error: PropTypes.string,
    success: PropTypes.string.isRequired,
    handleSubmit: PropTypes.func.isRequired,
  };

  render() {
    const { field, handleSubmit, className, asyncValidating, submitting, hasError, hasSuccess, error, success, } = this.props
    return(
      <div className={className}>
        <div className="col-md-6">
          <label className="control-label sr-only">Username</label>
          <div className="input-group">
            <input type="text" className="form-control" placeholder="Choose a username" disabled={submitting || asyncValidating} {...field} />
            <span className="input-group-btn input-group-btn-right" style={{left: '1px'}}>
              <button type="button" className="btn btn-default" disabled={submitting || asyncValidating} onClick={handleSubmit}>
                <span>Check availability</span>
                <span>{' '}</span>
                { hasError ?
                <span className="glyphicon glyphicon-remove text-danger" aria-hidden="true"></span> :
                hasSuccess ?
                <span className="glyphicon glyphicon-ok text-success" aria-hidden="true"></span> :
                <span className="glyphicon glyphicon-search text-info" aria-hidden="true"></span>
                }
              </button>
            </span>
          </div>
        </div>
        <div className="col-md-12">
        { asyncValidating ?
          <p className="help-block help-info">Checking availability...</p> :
          hasError || hasSuccess ?
          <div className="help-block">
            <Markdown className="help-block">{ error || success }</Markdown>
          </div> :
          <p className="help-block help-info">Use between 2 and 12 numbers, letters, hypens, underscores, and dots.</p>
        }
        </div>
      </div>
    )
  }
}

export default Field
